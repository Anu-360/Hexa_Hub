import './App.css'
import{BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HexaHubLandingPage from './Components/LandingPage/HomePage';
import SignInPage from './Components/SignInPage/Signin';
import Dashboard from './Components/AdminView/DashBoard/DashBoard';
import { ThemeProvider } from './Components/ThemeContext';
import Employee from './Components/AdminView/EmployeePage/Employee';

function App() {
  return (
    <ThemeProvider >
    <Router>
      <Routes>
        <Route path = "/admin/Dashboard" element={<Dashboard />}/>
        <Route path="/" element={<HexaHubLandingPage />} />
        <Route path="/signin" element={<SignInPage />} />
        <Route path='/employee' element={<Employee/>}/>
      </Routes>
    </Router>
    </ThemeProvider>
  )
}

export default App
