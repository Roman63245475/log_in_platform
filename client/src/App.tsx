import "./index.css";
import {useEffect, useState} from "react";
import {useLocation} from "react-router";
import UpBar from "@/UpBar.tsx";
import {ClientUserDTO, UserClient} from "@/api/Api.ts";

export function App() {
  const location = useLocation();
  const username: string | undefined = location.state?.username;
  const [users, setUsers] = useState<ClientUserDTO[]>([]);

  useEffect(() => {
    if (!username) return;
    new UserClient().users().then(setUsers);
  }, [username]);

  return (
    <div className="app">
      <UpBar/>
      {username && (
        <>
          <p>Logged in as {username}</p>
          <p>Users list:</p>
          <ul>
            {users.map(u => <li key={u.id}>{u.username}</li>)}
          </ul>
        </>
      )}
    </div>
  );
}

export default App;
