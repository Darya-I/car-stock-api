import axios from "../axios/axios";
import useAuth from "./useAuth";

const useRefreshToken = () => {
    const { setAuth } = useAuth();

    const refresh = async () => {
        const response = await axios.post('/api/Auth/Refresh',{}, {
            withCredentials: true
        });
        console.log(JSON.stringify(response));
        setAuth(prev => {
            const newAuth = { ...prev, accessToken: response.data }; // Убедись, что ключ верный
            localStorage.setItem("auth", JSON.stringify(newAuth)); // Сохраняем в localStorage
            return newAuth;
        });
        return response.data.token;
    }
    return refresh;
}

export default useRefreshToken;