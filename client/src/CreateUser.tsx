import {useState} from "react";
import {Api} from "@/api/Api.ts";
import {canManageUsers, UserRole} from "@/UserRole.ts";

const ROLES = [UserRole.User, UserRole.SuperUser, UserRole.Admin];

interface CreateUserProps {
    menuOpen: boolean;
    setMenuOpen: (isOpen: boolean) => void;
    onSuccess: () => void;
    authRole?: UserRole;
}

export function CreateUser({menuOpen, setMenuOpen, onSuccess, authRole}: CreateUserProps) {

    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [role, setRole] = useState(UserRole.User);

    const isAdmin = authRole === UserRole.Admin;

    if (!canManageUsers(authRole)) return null;

    async function register() {
        const token = localStorage.getItem("authToken");
        const api = new Api({
            baseApiParams: {headers: {Authorization: `Bearer ${token}`}},
        });
        try {
            // SuperUser can only create regular users
            await api.register.loginRegister({username, password, role: isAdmin ? role : UserRole.User});
            setUsername('');
            setPassword('');
            setRole(UserRole.User);
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
                <p>Role:</p>
                {isAdmin ? (
                    <select value={role} onChange={(e) => setRole(Number(e.target.value))}>
                        {ROLES.map(r => <option key={r} value={r}>{UserRole[r]}</option>)}
                    </select>
                ) : (
                    <input value={UserRole[UserRole.User]} disabled/>
                )} <br/>
                <button onClick={register}>Submit</button>
            </div>
        </div>
    )
}
