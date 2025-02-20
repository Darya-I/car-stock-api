import { createContext, useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";

const AuthContext = createContext({});

export const AuthProvider = ({ children }) => {
    const [auth, setAuth] = useState(() => {
        const storedAuth = localStorage.getItem("auth");
        return storedAuth ? JSON.parse(storedAuth) : {};
    });

    const [permission, setPermission] = useState([]);

    useEffect(() => {
        if (auth?.accessToken) {
            try {
                const decoded = jwtDecode(auth.accessToken);
                setPermission(decoded.Permission || []);
            } catch (err) {
                setPermission([]);
            }
        } else {
            setPermission([]);
        }
    }, [auth]); // Декодируем токен при изменении auth

    useEffect(() => {
        if (auth?.accessToken) {
            localStorage.setItem("auth", JSON.stringify(auth));
        } else {
            localStorage.removeItem("auth");
        }
    }, [auth]); // Сохраняем auth в localStorage при изменении

    return (
        <AuthContext.Provider value={{ auth, setAuth, permission }}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthContext;
