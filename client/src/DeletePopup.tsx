import {Api, type ClientUserDTO} from "@/api/Api.ts";

interface DeletePopupProps {
    userToDelete: ClientUserDTO,
    setUserToDelete: (user: ClientUserDTO | null) => void,
    onSuccess: () => void
}

export function DeletePopup({userToDelete, setUserToDelete, onSuccess}: DeletePopupProps) {

    async function deleteUser(){
        const token = localStorage.getItem("authToken");
        const api = new Api({
            baseApiParams: {headers: {Authorization: `Bearer ${token}`}}
        })
        try{
            if(!userToDelete.id) return;
            await api.users.userDeleteUser(userToDelete.id);
            setUserToDelete(null);
            onSuccess();
        }catch(error){
            console.error("Deletion failed: ", error);
        }
    }

    return (
        <div className={"pop-up-bg flex-container-v"} onClick={(e) => {
            setUserToDelete(null);
        }}>
            <div className={"pop-up flex-container-v"} onClick={(e) => {
                e.stopPropagation()
            }}>
                <h1>Confirmation</h1>
                <p>Are you sure you want to delete {userToDelete.username} </p>
                <button onClick={deleteUser}>Confirm</button>
                <button onClick={() => setUserToDelete(null)}>Cancel</button>
            </div>
        </div>
    )
}