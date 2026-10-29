import "./index.css";
import {useEffect, useState} from "react";
import {useLocation, useNavigate} from "react-router";
import UpBar from "@/UpBar.tsx";
import {Api, type ClientUserDTO} from "@/api/Api.ts";
import {jwtDecode} from "jwt-decode";

export function App() {
  const token = localStorage.getItem("authToken");
  const navigate = useNavigate();
  let username = "";
  if (token) {
    try{
      const decodedToken = jwtDecode(token) as any;
      username = decodedToken.username;
    }catch (error){
      console.error("Invalid token format");
    }
  }
  const [users, setUsers] = useState<ClientUserDTO[]>([]);

  const logout = () => {
      localStorage.removeItem("authToken");
      navigate("/login");
  };

  useEffect(() => {
    if (!token) return;
    const api = new Api({
      baseApiParams: {headers: {Authorization: `Bearer ${token}`}},
    });
    api.users.userUsers().then(result => setUsers(result.data));
  }, [token]);

  return (
    <div className="app">
      {token ? (
        <>
          <p>Logged in as {username}</p>
          <button onClick={logout}>Log out</button>
          <p>Users list:</p>
          <ul>
            {users.map(u => <li key={u.id}>{u.username}</li>)}
          </ul>
        </>
      ) : (
          <UpBar/>
      )}
    </div>
  );
}

export default App;
