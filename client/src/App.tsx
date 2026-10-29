import "./index.css";
import {useEffect, useState} from "react";
import {useLocation} from "react-router";
import UpBar from "@/UpBar.tsx";
import {Api, type ClientUserDTO} from "@/api/Api.ts";

export function App() {
  const location = useLocation();
  const username: string | undefined = location.state?.username;
  const [users, setUsers] = useState<ClientUserDTO[]>([]);

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (!username || !token) return;
    const api = new Api({
      baseApiParams: {headers: {Authorization: `Bearer ${token}`}},
    });
    api.users.userUsers().then(result => setUsers(result.data));
  }, [username]);

  return (
    <div className="app">
      <UpBar/>
      {username && (
        <>
          <p>Logged in as {username}</p>
          <ul>
            {users.map(u => <li key={u.id}>{u.username}</li>)}
          </ul>
        </>
      )}
    </div>
  );
}

export default App;
