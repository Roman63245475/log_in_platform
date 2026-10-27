import {useState} from "react";
import {LoginClient, UserDTO} from "@/api/Api.ts";

const Login = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [message, setMessage] = useState("");

    const login = async () => {
        const user_dto = new UserDTO({username: username, password: password});
        try {
            const result = await new LoginClient().login(user_dto);
            setMessage(`Logged in as ${result}`);
            console.log(message)
        } catch {
            setMessage("Check if username and password are correct");
        }
    }

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        login();
    }

    return (
        <form onSubmit={handleSubmit}>
            <input
                type="text"
                placeholder="username"
                value={username}
                onChange={e => setUsername(e.target.value)}
            />
            <input
                type="password"
                placeholder="password"
                value={password}
                onChange={e => setPassword(e.target.value)}
            />
            <button type="submit">Log in</button>
            <p>{message}</p>
        </form>
    )
}

export default Login
