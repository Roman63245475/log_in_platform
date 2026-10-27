import {useNavigate} from "react-router";

export default function UpBar(){
    const navigate = useNavigate();
    return (
        <ul className={'upbar'}>
            <li><button onClick={() => navigate("login")}>Login</button></li>
        </ul>
    )
}
