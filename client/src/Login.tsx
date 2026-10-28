import {useState} from "react";
import {useNavigate} from "react-router";
import {Api} from "@/api/Api.ts";

const Login = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [message, setMessage] = useState("");
    const navigate = useNavigate();

    const login = async () => {
        try {
            const result = await new Api().login.loginLogin({username, password});
            localStorage.setItem("authToken", result.data);
            navigate("/", {state: {username}});
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
