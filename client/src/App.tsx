import "./index.css";
import {useEffect, useState} from "react";
import {useLocation, useNavigate} from "react-router";
import UpBar from "@/UpBar.tsx";
import {Api, type ClientUserDTO} from "@/api/Api.ts";
import {jwtDecode} from "jwt-decode";
import {CreateUser} from "@/CreateUser.tsx";
import {DeletePopup} from "@/DeletePopup.tsx";

export function App() {
  const token = localStorage.getItem("authToken");
  const navigate = useNavigate();

  const [authUser, setAuthUser] = useState({username: "", role: ""})
  const [users, setUsers] = useState<ClientUserDTO[]>([]);

  const [refresh, setRefresh] = useState(false);
  const [userToDelete, setUserToDelete] = useState<ClientUserDTO | null>(null);
  const [createMenuOpen, setCreateMenuOpen] = useState(false);

  const logout = () => {
      localStorage.removeItem("authToken");
      navigate("/login");
  };

  useEffect(() => {
    if (!token) return;

    let currentRole = "";

    try{
      const decodedToken = jwtDecode(token) as any;
      currentRole = decodedToken.role;
      setAuthUser({username: decodedToken.username, role: decodedToken.role});
    }catch (error){
      console.error("Invalid token format");
      return;
    }

    const api = new Api({
      baseApiParams: {headers: {Authorization: `Bearer ${token}`}},
    });

    if (currentRole === "3" || currentRole === "Admin") {
      api.users.userUsers().then(result => setUsers(result.data));
    }else{
      api.usersRole.userUsersRole({role: "User"}).then(result => setUsers(result.data));
    }

  }, [token, refresh]);
  return (
    <div className="app">
      {token ? (
        <>
        {createMenuOpen && (<CreateUser menuOpen={createMenuOpen} setMenuOpen={setCreateMenuOpen} onSuccess={() => setRefresh(!refresh)} />)}
          {userToDelete && (<DeletePopup userToDelete={userToDelete} setUserToDelete={setUserToDelete} onSuccess={() => setRefresh(!refresh)} />)}
          <p>Logged in as {authUser.username}</p>
          <button onClick={logout}>Log out</button>
          {authUser.role != "1" && (
            <button onClick={() => setCreateMenuOpen(true)}>Create User</button>
          )
          }
          <p>Users list:</p>
          <ul>
            {users.filter(u => u.username !== authUser.username).map(u =>
              <li key={u.id}>{u.username} Role: {u.role_name}
              {authUser.role != "1" && (
                  <>
                    <button>Edit</button>
                    <button onClick={() => {setUserToDelete(u)}}>Delete</button>
                  </>
              )}
            </li>)}
          </ul>
        </>
      ) : (
          <UpBar/>
      )}
    </div>
  );
}

export default App;
