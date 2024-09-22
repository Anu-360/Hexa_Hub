import './App.css'
import{BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HexaHubLandingPage from './Components/LandingPage/HomePage';
import SignInPage from './Components/SignInPage/Signin';
import Dashboard from './Components/AdminView/DashBoard/DashBoard';
import { ThemeProvider } from './Components/ThemeContext';
import Employee from './Components/AdminView/EmployeePage/Employee';
import UpdateUser from './Components/AdminView/EmployeePage/UpdateUser'
import UserDetails from './Components/AdminView/EmployeePage/EmployeeInfo'

function App() {
  return (
    <ThemeProvider >
    <Router>
      <Routes>
        <Route path = "/admin/Dashboard" element={<Dashboard />}/>
        <Route path="/" element={<HexaHubLandingPage />} />
        <Route path="/signin" element={<SignInPage />} />
        <Route path='/employee' element={<Employee/>}/>
        <Route path="/user/update/:id" element={<UpdateUser />} />
        <Route path="/user/:id" element = {<UserDetails/>}/>

      </Routes>
    </Router>
    </ThemeProvider>
  )
}

export default App
