import React, { useState, useEffect, useMemo } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { useTheme } from '../../ThemeContext';
import PaginationRounded from '../../Utils/Pagination';
import usePagination from '../../Utils/usePagination';
import RadioButton from '../../Utils/RadioButton';
import { jwtToken } from '../../Utils/utils';
import ConfirmationDialog from '../../Utils/Dialog';
import Cookies from 'js-cookie';
import AddIcon from '@mui/icons-material/Add';
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
    Button,
} from '@mui/material';
import InfoIcon from '@mui/icons-material/Info';
import SearchIcon from '@mui/icons-material/Search';
import DeleteIcon from '@mui/icons-material/Delete';
import { jsPDF } from "jspdf";
import "jspdf-autotable";
import PictureAsPdfIcon from '@mui/icons-material/PictureAsPdf';

const token = Cookies.get('token');
if (token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

export default function Employee() {
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
                setEmployees(response.data.$values || []);
            } catch (error) {
                console.error("Error fetching employees:", error);
            }
        };

        fetchData();
    }, []);

    const filteredEmployees = useMemo(() => {
        return employees.filter(employee => {
            const searchLower = searchTerm.toLowerCase();
            return (
                (employee.userName?.toLowerCase().includes(searchLower) || "") ||
                (employee.userId?.toString().includes(searchLower) || "") ||
                (employee.branch?.toLowerCase().includes(searchLower) || "") ||
                (employee.dept?.toLowerCase().includes(searchLower) || "") ||
                (employee.designation?.toLowerCase().includes(searchLower) || "")
            );
        });
    }, [employees, searchTerm]);

    const { currentItems, paginate, pageCount, currentPage, setCurrentPage } = usePagination(itemsPerPage, filteredEmployees);

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

    const handleSearch = (event, newValue) => {
        setSearchTerm(newValue);
        setCurrentPage(1);
    };

    const handleRadioBtn = (newValue) => {
        setSelectedValue(newValue);
    };

    const logo = "/Images/logo.png";
    const generatePDF = async () => {
        const doc = new jsPDF({
            orientation: 'landscape',
            unit: 'pt',
            format: 'a4'
        });

        const tableColumn = ["UserId", "Name", "Email", "Department", "Designation", "Phone Number", "Address", "Branch"];
        const tableRows = [];

        filteredEmployees.forEach(employee => {
            const employeeData = [
                employee.userId,
                employee.userName,
                employee.userMail,
                employee.dept,
                employee.designation,
                employee.phoneNumber,
                employee.address,
                employee.branch
            ];
            tableRows.push(employeeData);
        });
        let img;
    try {
        img = await loadImage(logo);
    } catch (error) {
        console.error("Error loading the logo image:", error);
        return;
    }
        doc.autoTable({
            head: [tableColumn],
            body: tableRows,
            startY: 80,
            theme: 'grid',
            styles: { fontSize: 8, cellPadding: 1 },
            columnStyles: {
                0: { cellWidth: 40 },
                1: { cellWidth: 80 },
                2: { cellWidth: 100 },
                3: { cellWidth: 80 },
                4: { cellWidth: 80 },
                5: { cellWidth: 80 },
                6: { cellWidth: 100 },
                7: { cellWidth: 80 }
            },
            didDrawPage: function (data) {
                if (doc.internal.getNumberOfPages() === 1) {
                    doc.addImage(img, 'PNG', 10, 10, 30, 30);
                doc.setFontSize(18);
                doc.text("HexaHub", 50, 30);
                doc.text("Employee List", 40, 60);
                }
            },
        });

        doc.save("employee_list.pdf");
    };
    const loadImage = (url) => {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.src = url;
            img.onload = () => {
                console.log("Image loaded successfully");
                resolve(img);
            };
            img.onerror = (err) => {
                console.error("Error loading image:", err);
                reject(err);
            };
        });
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
                            options={employees.map((option) => option.userName)}
                            onInputChange={handleSearch}
                            renderInput={(params) => (
                                <TextField
                                    placeholder='by Name, Id, Branch, Dept, Designation'
                                    {...params}
                                    label="Search "
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
                            <Link to={'/admin/employee/add'}>
                                <IconButton>
                                    <AddIcon />
                                </IconButton>
                            </Link>

                            {selectedValue != null && (
                                <>
                                    <IconButton onClick={() => handleDeleteConfirmation(selectedValue)}>
                                        <DeleteIcon />
                                    </IconButton>
                                </>
                            )}
                            <IconButton onClick={generatePDF} >
                                <PictureAsPdfIcon />
                            </IconButton>
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
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {currentItems.length > 0 ? (
                                    currentItems.map((employee) => (
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
                                            <TableCell>
                                                <Link to={`${employee.userId}`}>
                                                    <IconButton>
                                                        <InfoIcon />
                                                    </IconButton>
                                                </Link>
                                            </TableCell>
                                        </TableRow>
                                    ))
                                ) : (
                                    <TableRow>
                                        <TableCell colSpan={10} align="center">No employees found</TableCell>
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </TableContainer>

                    <PaginationRounded count={pageCount} page={currentPage} onChange={(_, page) => setCurrentPage(page)} />
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