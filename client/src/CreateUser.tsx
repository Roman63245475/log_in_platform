import {useState} from "react";
import {Api, type ClientUserDTO} from "@/api/Api.ts";
import {canManageUsers, UserRole} from "@/UserRole.ts";

const ROLES = [UserRole.User, UserRole.SuperUser, UserRole.Admin];

interface CreateUserProps {
    menuOpen: boolean;
    setMenuOpen: (isOpen: boolean) => void;
    onSuccess: () => void;
    authRole?: UserRole;
    // when set, the form edits this user instead of creating a new one
    userToEdit?: ClientUserDTO;
}

export function CreateUser({menuOpen, setMenuOpen, onSuccess, authRole, userToEdit}: CreateUserProps) {

    const [username, setUsername] = useState(userToEdit?.username ?? '');
    const [password, setPassword] = useState('');
    const [role, setRole] = useState<UserRole>(userToEdit?.role ?? UserRole.User);

    const isAdmin = authRole === UserRole.Admin;
    const isEdit = userToEdit !== undefined;

    if (!canManageUsers(authRole)) return null;

    async function submit() {
        const token = localStorage.getItem("authToken");
        const api = new Api({
            baseApiParams: {headers: {Authorization: `Bearer ${token}`}},
        });
        // SuperUser can't pick a role: new users are User, edited users keep their role
        const sentRole = isAdmin ? role : (userToEdit?.role ?? UserRole.User);
        try {
            if (isEdit) {
                await api.users.userEditUser(userToEdit.id!, {username, role: sentRole});
            } else {
                await api.register.loginRegister({username, password, role: sentRole});
            }
            setUsername('');
            setPassword('');
            setRole(UserRole.User);
            setMenuOpen(false);
            onSuccess();
        } catch (err) {
            console.log(isEdit ? "Failed to edit user " : "Failed to create user ", err);
        }
    }

    return (
        <div className={"pop-up-bg flex-container-v"} onClick={(e) => {
            setMenuOpen(false);
        }}>
            <div className={"pop-up flex-container-v"} onClick={(e) => {
                e.stopPropagation()
            }}>
                <h1>{isEdit ? "Edit User" : "Create User"}</h1>
                <p>Username:</p>
                <input value={username} onChange={(e) => setUsername(e.target.value)}/> <br/>
                {!isEdit && (
                    <>
                        <p>Password:</p>
                        <input value={password} onChange={(e) => setPassword(e.target.value)} type={"password"}/> <br/>
                    </>
                )}
                <p>Role:</p>
                {isAdmin ? (
                    <select value={role} onChange={(e) => setRole(Number(e.target.value))}>
                        {ROLES.map(r => <option key={r} value={r}>{UserRole[r]}</option>)}
                    </select>
                ) : (
                    <input value={UserRole[role]} disabled/>
                )} <br/>
                <button onClick={submit}>Submit</button>
            </div>
        </div>
    )
}
