/* eslint-disable no-unused-vars */
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import Header from '../AdminHeader';
import Navbar from '../AdminNavBar';
import { useTheme } from '../../ThemeContext';
import PaginationRounded from '../../Utils/Pagination';
import usePagination from '../../Utils/usePagination';
import RadioButton from '../../Utils/RadioButton';
import { jwtToken } from '../../Utils/utils';
import ConfirmationDialog from '../../Utils/Dialog';
import Cookies from 'js-cookie';
import {
    Box,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper,
    Typography,
    Toolbar,
    Autocomplete,
    TextField,
    IconButton,
} from '@mui/material';
import InfoIcon from '@mui/icons-material/Info';
import SearchIcon from '@mui/icons-material/Search';
import FilterListIcon from '@mui/icons-material/FilterList';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

const token = Cookies.get('token');
if (token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

export default function Employee() {
    const [mobileOpen, setMobileOpen] = useState(false);
    const { darkMode } = useTheme();
    const itemsPerPage = 10;
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedValue, setSelectedValue] = useState(null);
    const [employees, setEmployees] = useState([]);
    const [openDialog, setOpenDialog] = useState(false);
    const [deleteId, setDeleteId] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            const decodedToken = jwtToken();
            if (!decodedToken) {
                console.error('No valid token found.');
                return;
            }
            try {
                const response = await axios.get('https://localhost:7287/api/Users/role?role=Employee');
                console.log('Fetched Employees Response:', JSON.stringify(response.data));
                setEmployees(response.data.$values || []);
            } catch (error) {
                console.error("Error fetching employees:", error);
            }
        };

        fetchData();
    }, []);

    const handleDeleteConfirmation = (id) => {
        setDeleteId(id);
        setOpenDialog(true);
    };

    const handleDelete = async () => {
        if (deleteId) {
            try {
                await axios.delete(`https://localhost:7287/api/Users/${deleteId}`);
                setEmployees(employees.filter(employee => employee.userId !== deleteId));
                setOpenDialog(false);
                setDeleteId(null);
            } catch (error) {
                console.error("Error deleting employee:", error);
            }
        }
    };

    const handleDrawerToggle = () => {
        setMobileOpen(!mobileOpen);
    };

    const filteredEmployees = employees.filter(employee => {
        const searchLower = searchTerm.toLowerCase();
        return (
            employee.userName.toLowerCase().includes(searchLower) ||
            employee.userId.toString().includes(searchLower) 
        );
    });
    const { currentItems, paginate, pageCount } = usePagination(itemsPerPage, filteredEmployees);

    const handleRadioBtn = (newValue) => {
        console.log(`Selected value changed to: ${newValue}`);
        setSelectedValue(newValue);
    };

    return (
        <Box
            sx={{
                bgcolor: darkMode ? 'background.paper' : 'background.default',
                display: 'flex',
                flexDirection: 'column',
                minHeight: 'fit-content',
            }}
        >
            <Header handleDrawerToggle={handleDrawerToggle} />
            <Navbar mobileOpen={mobileOpen} handleDrawerToggle={handleDrawerToggle} />
            <Box sx={{ display: 'flex', flex: 1 }}>
                <Box
                    component="main"
                    sx={{
                        flexGrow: 1,
                        p: 3,
                        width: { sm: `calc(100% - 240px)` },
                        marginLeft: { sm: '240px' },
                    }}
                >
                    <Toolbar />
                    <Typography variant="h4" gutterBottom>
                        Employee Management
                    </Typography>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                        <Autocomplete
                            freeSolo
                            disableClearable
                            options={currentItems.map((option) => (option.userName))}
                            onInputChange={(e, newValue) => {
                                setSearchTerm(newValue);
                            }}
                            renderInput={(params) => (
                                <TextField
                                    {...params}
                                    label="Search"
                                    InputProps={{
                                        ...params.InputProps,
                                        type: 'search',
                                        endAdornment: (
                                            <IconButton>
                                                <SearchIcon />
                                            </IconButton>
                                        ),
                                    }}
                                    sx={{ width: 300 }}
                                />
                            )}
                        />
                        <Box>
                            <IconButton>
                                <FilterListIcon />
                            </IconButton>
                            {selectedValue != null && (
                                <>
                                    <Link to={`/user/update/${selectedValue}`}>
                                    <IconButton>
                                        <EditIcon />
                                    </IconButton>
                                    </Link>
                                    <IconButton onClick={() => handleDeleteConfirmation(selectedValue)}>
                                        <DeleteIcon />
                                    </IconButton>
                                </>
                            )}
                        </Box>
                    </Box>
                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="employee table">
                            <TableHead>
                                <TableRow>
                                    <TableCell></TableCell>
                                    <TableCell>UserId</TableCell>
                                    <TableCell>Name</TableCell>
                                    <TableCell>Email</TableCell>
                                    <TableCell>Department</TableCell>
                                    <TableCell>Designation</TableCell>
                                    <TableCell>Phone Number</TableCell>
                                    <TableCell>Address</TableCell>
                                    <TableCell>Branch</TableCell>
                                    <TableCell>Role</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {currentItems.map((employee) => (
                                    <TableRow key={employee.userId}>
                                        <TableCell>
                                            <RadioButton
                                                selectedValue={selectedValue}
                                                onChange={handleRadioBtn}
                                                value={employee.userId}
                                            />
                                        </TableCell>
                                        <TableCell>{employee.userId}</TableCell>
                                        <TableCell>{employee.userName}</TableCell>
                                        <TableCell>{employee.userMail}</TableCell>
                                        <TableCell>{employee.dept}</TableCell>
                                        <TableCell>{employee.designation}</TableCell>
                                        <TableCell>{employee.phoneNumber}</TableCell>
                                        <TableCell>{employee.address}</TableCell>
                                        <TableCell>{employee.branch}</TableCell>
                                        <TableCell>{employee.user_Type === 1 ? 'Admin' : 'Employee'}</TableCell>
                                        <TableCell>
                                            <Link to={`/user/${employee.userId}`}>
                                                <IconButton>
                                                    <InfoIcon />
                                                </IconButton>
                                            </Link>
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                    <PaginationRounded count={pageCount} onChange={paginate} />
                </Box>
            </Box>
            <ConfirmationDialog
                open={openDialog}
                onClose={() => setOpenDialog(false)}
                onConfirm={handleDelete}
                title="Confirm Deletion"
                message="Are you sure you want to delete this employee?"
            />
        </Box>
    );
}
