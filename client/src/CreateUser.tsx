import {useState} from "react";
import {Api} from "@/api/Api.ts";

interface CreateUserProps {
    menuOpen: boolean;
    setMenuOpen: (isOpen: boolean) => void;
    onSuccess: () => void;
}

export function CreateUser({menuOpen, setMenuOpen, onSuccess}: CreateUserProps) {

    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    async function register() {
        const token = localStorage.getItem("authToken");
        const api = new Api({
            baseApiParams: {headers: {Authorization: `Bearer ${token}`}},
        });
        try {
            await api.register.loginRegister({username, password});
            setUsername('');
            setPassword('');
            setMenuOpen(false);
            onSuccess();
        } catch (err) {
            console.log("Failed to create user ", err);
        }
    }

    return (
        <div className={"pop-up-bg flex-container-v"} onClick={(e) => {
            setMenuOpen(false);
        }}>
            <div className={"pop-up flex-container-v"} onClick={(e) => {
                e.stopPropagation()
            }}>
                <h1>Create User</h1>
                <p>Username:</p>
                <input value={username} onChange={(e) => setUsername(e.target.value)}/> <br/>
                <p>Password:</p>
                <input value={password} onChange={(e) => setPassword(e.target.value)} type={"password"}/> <br/>
                <button onClick={register}>Submit</button>
            </div>
        </div>
    )
}