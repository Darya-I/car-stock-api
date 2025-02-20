import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import HomePage from './home_page/HomePage';
import CarsPage from './pages/Cars/CarsPage';
import UsersPage from './pages/UsersPage';
import AccountPage from './pages/AccountPage';
import { ChakraProvider, defaultSystem } from '@chakra-ui/react';
import GetCarsPage from './pages/Cars/GetCarsPage';
import CreateCarPage from './pages/Cars/CreateCarPage';
import EditCarPage from './pages/Cars/EditCarPage';
import Register from './Auth/Register';
import Login from './Auth/Login';
import RequireAuth from './Auth/RequireAuth';
import { ToastContainer } from 'react-toastify';
import ForbiddenPage from './pages/ForbiddenPage';
function App() {
  return (
    <>
      <ToastContainer />
      <ChakraProvider value={defaultSystem}>
        <Router>
          <Routes>
            <Route path='/' Component={HomePage} />
            <Route path='/register' Component={Register} />
            <Route path='/login' Component={Login} />
            <Route path='/forbidden' Component={ForbiddenPage} />
            {/* Защищенные маршруты */}
            <Route element={<RequireAuth />}>
              <Route path='/cars' Component={CarsPage} />
              <Route path='/getcars' Component={GetCarsPage} />

              {/* Доступ только с правом "CanEditCar" */}
              <Route element={<RequireAuth requiredPolicy="CanEditCar" />}>
                <Route path='/edit-car/:id' Component={EditCarPage} />
              </Route>

              <Route element={<RequireAuth requiredPolicy="CanCreateCar" />}>
                <Route path='/createcar' Component={CreateCarPage} />
              </Route>

              <Route path='/users' Component={UsersPage} />
              <Route path='/account' Component={AccountPage} />
            </Route>
          </Routes>
        </Router>
      </ChakraProvider>
    </>

  )
}

export default App
