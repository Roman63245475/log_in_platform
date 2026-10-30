import "./index.css";
import {useEffect, useState} from "react";
import {useLocation, useNavigate} from "react-router";
import UpBar from "@/UpBar.tsx";
import {Api, type ClientUserDTO} from "@/api/Api.ts";
import {jwtDecode} from "jwt-decode";
import {CreateUser} from "@/CreateUser.tsx";
import {DeletePopup} from "@/DeletePopup.tsx";
import {canManageUsers, roleFromName, UserRole} from "@/UserRole.ts";

export function App() {
  const token = localStorage.getItem("authToken");
  const navigate = useNavigate();

  const [authUser, setAuthUser] = useState<{username: string, role?: UserRole}>({username: ""})
  const [users, setUsers] = useState<ClientUserDTO[]>([]);

  const [refresh, setRefresh] = useState(false);
  const [userToDelete, setUserToDelete] = useState<ClientUserDTO | null>(null);
  const [createMenuOpen, setCreateMenuOpen] = useState(false);

  const canManage = canManageUsers(authUser.role);

  const logout = () => {
      localStorage.removeItem("authToken");
      navigate("/login");
  };

  useEffect(() => {
    if (!token) return;

    try{
      const decodedToken = jwtDecode(token) as any;
      setAuthUser({username: decodedToken.username, role: roleFromName(decodedToken.role)});
    }catch (error){
      console.error("Invalid token format");
      return;
    }

    const api = new Api({
      baseApiParams: {headers: {Authorization: `Bearer ${token}`}},
    });

    // the server returns only the users visible to the logged-in role
    api.users.userUsers().then(result => setUsers(result.data));

  }, [token, refresh]);
  return (
    <div className="app">
      {token ? (
        <>
        {createMenuOpen && (<CreateUser menuOpen={createMenuOpen} setMenuOpen={setCreateMenuOpen} onSuccess={() => setRefresh(!refresh)} authRole={authUser.role} />)}
          {userToDelete && (<DeletePopup userToDelete={userToDelete} setUserToDelete={setUserToDelete} onSuccess={() => setRefresh(!refresh)} />)}
          <p>Logged in as {authUser.username}</p>
          <button onClick={logout}>Log out</button>
          {canManage && (
            <button onClick={() => setCreateMenuOpen(true)}>Create User</button>
          )
          }
          <p>Users list:</p>
          <ul>
            {users.filter(u => u.username !== authUser.username).map(u =>
              <li key={u.id}>{u.username} Role: {UserRole[u.role!]}
              {canManage && (
                  <button onClick={() => {setUserToDelete(u)}}>Delete</button>
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
