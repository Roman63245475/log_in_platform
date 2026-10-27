import "./index.css";
import {useNavigate} from "react-router";

export function App() {
  const navigate = useNavigate();
  return (
    <div className="app">
      <button onClick={() => navigate("login")}>Login</button>
    </div>
  );
}

export default App;
