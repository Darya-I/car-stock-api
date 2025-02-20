import { useLocation, Navigate, Outlet } from "react-router-dom";
import useAuth from '../hooks/useAuth';

const RequireAuth = ({ requiredPolicy }) => {
    const { auth, permission } = useAuth();
    const location = useLocation();

    if (!auth?.email) {
        return <Navigate to='/login' state={{from: location}} replace />;
    }

    if (requiredPolicy && !permission.includes(requiredPolicy)) {
        return <Navigate to='/forbidden' replace />
    }

    return <Outlet />
    
}

export default RequireAuth;