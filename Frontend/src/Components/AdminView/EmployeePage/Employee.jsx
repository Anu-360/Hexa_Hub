import Header from '../AdminHeader';
import Navbar from '../AdminNavBar';
import { useState } from 'react';
import { useTheme } from '../../ThemeContext';
import { Box } from '@mui/material';

export default function Employee() {
    const [mobileOpen, setMobileOpen] = useState(false);
    const { darkMode } = useTheme(); 
    const handleDrawerToggle = () => {
        setMobileOpen(!mobileOpen);
    };

    return (
        <Box 
        className=" h-fit" 
        sx={{
            bgcolor: darkMode ? 'background.paper' : 'background.default',
            color: darkMode ? 'text.primary' : 'text.primary',
            display: 'flex',
            flexDirection: 'column'
        }}
        >
        <Header handleDrawerToggle={handleDrawerToggle} />
        <Navbar mobileOpen={mobileOpen} handleDrawerToggle={handleDrawerToggle} />
        <Box sx={{ display: 'flex', flex: 1 }}>
            <Box component="main"  sx={{ flexGrow: 1, p: 10}}>
                {/* Add your employee content here */}
                <h1>HexaHub Users</h1>
                <p>This is the employee page content.</p>
            </Box>
        </Box>
    </Box>
    );
}
