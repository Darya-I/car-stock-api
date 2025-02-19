import { createContext, useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";

const AuthContext = createContext({});

export const AuthProvider = ({ children }) => {
    const [auth, setAuth] = useState({});
    const [permission, setPermission] = useState([]);

    useEffect(() => {
        if (auth?.accessToken) {
            try {
                const decoded = jwtDecode(auth.accessToken);
                setPermission(decoded.Permission || []);
                console.log({permission});
            } catch (err) {
                setPermission([]);
            }
        }
        else {
            setPermission([]);
        }
    }, [auth]) // декодируем при изменении auth

    useEffect(() => {
        console.log("Updated permission:", permission);
    }, [permission]);


    return (
        <AuthContext.Provider value={{auth, setAuth, permission}}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContext;