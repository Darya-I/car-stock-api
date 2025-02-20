import { useContext } from "react";
import AuthContext from "../context/AuthProvider";
import { useNavigate } from "react-router-dom";

const useLogout = () => {
    const { setAuth } = useContext(AuthContext);
    const navigate = useNavigate();

    const logout = async () => {
        try {
            // Очистка состояния аутентификации
            setAuth({});
            
            // Удаляем данные из localStorage
            localStorage.removeItem("auth");

            // Перенаправление на страницу логина
            navigate("/login", { replace: true });
        } catch (error) {
            console.error("Logout failed:", error);
        }
    };

    return logout;
};

export default useLogout;
