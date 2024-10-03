import './App.css'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HexaHubLandingPage from './Components/LandingPage/HomePage';
import SignInPage from './Components/SignInPage/Signin';
import Privacy from './Components/PrivacyTerms/Privacy';
import Terms from './Components/PrivacyTerms/Terms';
import Dashboard from './Components/AdminView/DashBoard/DashBoard';
import EmpDashboard from './Components/EmployeeView/EmpDashboard/EmpDashboard';
import ServiceRequest from './Components/EmployeeView/ServicePage/ServiceRequest';
import ReturnRequest from './Components/EmployeeView/ReturnRequest/ReturnRequest';
import EAssetPage from './Components/EmployeeView/EAssetPage/EAssetPage';
import Notification from './Components/EmployeeView/Notification/Notification';
import Profile from './Components/EmployeeView/Profile/Profile';
import Settings from './Components/EmployeeView/SettingsPage/SettingsPage';
import { ThemeProvider } from './Components/ThemeContext';
import EmpLink from './Components/AdminView/EmployeePage/EmpLink';
import AssetLink from './Components/AdminView/AssetPage/AssetLink';
import RequestLink from './Components/AdminView/AssetRequest/RequestLink';
import AllocationLink from './Components/AdminView/Allocation/AllocationLink';
import ReturnLink from './Components/AdminView/Return/ReturnLink';
import AuditLink from './Components/AdminView/Audit/AuditLink';
import ServiceLink from './Components/AdminView/ServicePage/ServiceLink';
import MaintenanceLink from './Components/AdminView/Maintenance/MaintenanceLink';
import Header from './Components/AdminView/AdminHeader';
import Navbar from './Components/AdminView/AdminNavBar';
import { Box } from '@mui/material';
import { useState } from 'react';
import { Outlet } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


const AdminLayout = ({ mobileOpen, handleDrawerToggle }) => (
  <>
    <Header handleDrawerToggle={handleDrawerToggle} />
    <Navbar mobileOpen={mobileOpen} handleDrawerToggle={handleDrawerToggle} />
    <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
      <Outlet />
    </Box>
  </>
);

function App() {
  const [mobileOpen, setMobileOpen] = useState(false);

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  return (
    <ThemeProvider >
      <Router>
        <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
          <ToastContainer />

          <Routes>
            <Route path="/" element={<HexaHubLandingPage />} />
            <Route path="/signin" element={<SignInPage />} />
            <Route path="Privacy" element={<Privacy/>}/>
            <Route path="Terms" element={<Terms/>}/>
            <Route path="dashboard" element={<EmpDashboard />} />
            <Route path="ServiceRequest" element={<ServiceRequest />} />
            <Route path="ReturnRequest" element={<ReturnRequest />} />
            <Route path="Asset" element={<EAssetPage />} />
            <Route path="Notification" element={<Notification />} />
            <Route path="Profile" element={<Profile />} />
            <Route path="Settings" element={<Settings />} />

            <Route path="admin" element={<AdminLayout mobileOpen={mobileOpen} handleDrawerToggle={handleDrawerToggle} />}>
              <Route path="Dashboard" element={<Dashboard />} />
              <Route path="employee/*" element={<EmpLink />} />
              <Route path="asset/*" element={<AssetLink />} />
              <Route path="request/*" element={<RequestLink />} />
              <Route path="allocation/*" element={<AllocationLink />} />
              <Route path="return/*" element={<ReturnLink />} />
              <Route path="audit/*" element={<AuditLink />} />
              <Route path="service/*" element={<ServiceLink />} />
              <Route path="maintenance/*" element={<MaintenanceLink />} />
            </Route>
          </Routes>
        </Box>
      </Router>
    </ThemeProvider>
  )
}

export default App
