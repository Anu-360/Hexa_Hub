import React, { useState, useEffect, useMemo } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { useTheme } from '../../ThemeContext';
import PaginationRounded from '../../Utils/Pagination';
import usePagination from '../../Utils/usePagination';
import { jwtToken } from '../../Utils/utils';
import RadioButton from '../../Utils/RadioButton';
import Cookies from 'js-cookie';
import {
    Box,
    Table,
    TableBody,
    Select,
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
    Drawer,
    MenuItem,
    FormControl,
    InputLabel,
    Button,
} from '@mui/material';
import InfoIcon from '@mui/icons-material/Info';
import SearchIcon from '@mui/icons-material/Search';
import FilterListIcon from '@mui/icons-material/FilterList';
import EditIcon from '@mui/icons-material/Edit';
import { jsPDF } from "jspdf";
import "jspdf-autotable";
import PictureAsPdfIcon from '@mui/icons-material/PictureAsPdf';

const token = Cookies.get('token');
if (token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

export default function ReturnPage() {
    const { darkMode } = useTheme();
    const itemsPerPage = 10;
    const [searchTerm, setSearchTerm] = useState('');
    const [filterDrawerOpen, setFilterDrawerOpen] = useState(false);
    const [returnRequests, setReturnRequests] = useState([]);
    const [minDate, setMinDate] = useState('');
    const [maxDate, setMaxDate] = useState('');
    const [selectedStatus, setSelectedStatus] = useState('');
    const [selectedValue, setSelectedValue] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            const decodedToken = jwtToken();
            if (!decodedToken) {
                console.error('No valid token found.');
                return;
            }
            try {
                const response = await axios.get('https://localhost:7287/api/ReturnRequests/all');
                console.log('Fetched Return Response:', JSON.stringify(response.data));
                setReturnRequests(response.data.$values || []);
            } catch (error) {
                console.error('Error fetching Return:', error);
            }
        };

        fetchData();
    }, []);

    const filteredRequests = useMemo(() => {
        return returnRequests.filter((returnRequest) => {
            const searchLower = searchTerm.toLowerCase();
            const dateMatch = (!minDate || new Date(returnRequest.returnDate) >= new Date(minDate)) &&
                (!maxDate || new Date(returnRequest.returnDate) <= new Date(maxDate));
            return (
                (
                    returnRequest.userName.toLowerCase().includes(searchLower) ||
                    returnRequest.assetId.toString().includes(searchLower) ||
                    returnRequest.returnId.toString().includes(searchLower) ||
                    returnRequest.assetName.toLowerCase().includes(searchLower)) &&
                (selectedStatus ? returnRequest.returnStatusName === selectedStatus : true) &&
                dateMatch
            );
        });
    }, [returnRequests, searchTerm, minDate, maxDate, selectedStatus]);

    const { currentItems, setCurrentPage, currentPage, pageCount } = usePagination(itemsPerPage, filteredRequests);

    const handleSearch = (event, newValue) => {
        setSearchTerm(newValue);
        setCurrentPage(1);
    };

    
    const logo = "/Images/logo.png";
    const generatePDF = async() =>{
        const doc = new jsPDF({
            orientation: 'landscape',
            unit:'pt',
            format: 'a4'
        });

        const tableColumn = ["Return ID", "Asset Id", "Asset Name", "User Name","Return Date", "Status"];
        const tableRows = [];

        filteredRequests.forEach(returnRequest =>{
            const RequestData = [
                returnRequest.returnId,
                returnRequest.assetId,
                returnRequest.assetName,
                returnRequest.userName,
                returnRequest.returnDate,
                returnRequest.requestStatusName
            ];
            tableRows.push(RequestData);
        });
        try {
            const img = await loadImage(logo);
            doc.addImage(img, 'PNG', 10, 10, 30, 30);
        } catch (error) {
            console.error("Error loading the logo image:", error);
            return;
        }
        doc.autoTable({
            head: [tableColumn],
            body: tableRows,
            startY: 100,
            theme: 'grid',
            styles: { fontSize: 8, cellPadding: 1 },
            columnStyles: {
                0: { cellWidth: 60 },
                1: { cellWidth: 100 },
                2: { cellWidth: 80 },
                3: { cellWidth: 60 },
                4: { cellWidth: 80 },
                5: { cellWidth: 80 },
                6: { cellWidth: 70 }
            },
            didDrawPage :function(data){
                doc.setFontSize(18);
                doc.text("HexaHub", 50, 30);
                doc.setFontSize(20);
                doc.text("Asset Request List", 40, 60);
                doc.setFontSize(10);
                let filterText = `Filters: Date Range: ${minDate ? new Date(minDate).toLocaleDateString() : 'N/A'} - ${maxDate ? new Date(maxDate).toLocaleDateString() : 'N/A'}${selectedStatus ? `, Status: ${selectedStatus}` : ''}`;
                doc.text(filterText, 40, 80);
            },
        });
        doc.save("assetRequest_List.pdf");
    }
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

    const handleRadioBtn = (newValue) => {
        console.log(`Selected value changed to: ${newValue}`);
        setSelectedValue(newValue);
    };

    const toggleFilterDrawer = () => {
        setFilterDrawerOpen(!filterDrawerOpen);
    };

    const clearFilters = () => {
        setMinDate('');
        setMaxDate('');
        setSearchTerm('');
        setCurrentPage(1);
        setSelectedStatus('');
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
                        Return Requests
                    </Typography>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                        <Autocomplete
                            freeSolo
                            disableClearable
                            options={returnRequests.map((option) => option.assetName)}
                            onInputChange={handleSearch}
                            renderInput={(params) => (
                                <TextField
                                    placeholder="by Name, ID"
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
                            <IconButton onClick={toggleFilterDrawer}>
                                <FilterListIcon />
                            </IconButton>
                            <IconButton onClick={generatePDF}>
                                <PictureAsPdfIcon />
                            </IconButton>
                            {selectedValue != null && (
                                <Link to={`/admin/return/update/${selectedValue}`}>
                                    <IconButton>
                                        <EditIcon />
                                    </IconButton>
                                </Link>
                            )}
                        </Box>
                    </Box>

                    <Drawer anchor="right" open={filterDrawerOpen} onClose={toggleFilterDrawer}>
                        <Box sx={{ width: 250, padding: 2, marginTop: '64px' }}>
                            <Typography variant="h6">Filters</Typography>
                            <FormControl sx={{ minWidth: 150, mb: 2 }}>
                                <InputLabel>Status</InputLabel>
                                <Select
                                    value={selectedStatus}
                                    onChange={(e) => setSelectedStatus(e.target.value)}
                                    label="Status"
                                >
                                    <MenuItem value="">All</MenuItem>
                                    <MenuItem value="Sent">Sent</MenuItem>
                                    <MenuItem value="Rejected">Rejected</MenuItem>
                                    <MenuItem value="Approved">Approved</MenuItem>
                                    <MenuItem value="Returned">Returned</MenuItem>
                                </Select>
                            </FormControl>  
                            <Box sx={{ mb: 2 }}>
                                <Typography gutterBottom>Return Date</Typography>
                                <TextField
                                    label="Min Date"
                                    type="date"
                                    value={minDate}
                                    onChange={(e) => setMinDate(e.target.value)}
                                    sx={{ width: '100%', mb: 1 }}
                                />
                                <TextField
                                    label="Max Date"
                                    type="date"
                                    value={maxDate}
                                    onChange={(e) => setMaxDate(e.target.value)}
                                    sx={{ width: '100%' }}
                                />
                            </Box>
                            <Button variant="outlined" onClick={clearFilters}>
                                Clear Filters
                            </Button>
                        </Box>
                    </Drawer>

                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 650 }} aria-label="return requests table">
                            <TableHead>
                                <TableRow>
                                    <TableCell></TableCell>
                                    <TableCell>Return ID</TableCell>
                                    <TableCell>Asset ID</TableCell>
                                    <TableCell>Asset Name</TableCell>
                                    <TableCell>User Name</TableCell>
                                    <TableCell>Return Date</TableCell>
                                    <TableCell>Return Status</TableCell>
                                    <TableCell></TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                            {currentItems.length > 0 ? (
                                currentItems.map((returnRequest) => (
                                    <TableRow key={returnRequest.returnId}>
                                        <TableCell>
                                        <RadioButton
                                                selectedValue={selectedValue}
                                                onChange={handleRadioBtn}
                                                value={returnRequest.returnId}
                                            />
                                        </TableCell>
                                        <TableCell>{returnRequest.returnId}</TableCell>
                                        <TableCell>{returnRequest.assetId}</TableCell>
                                        <TableCell>{returnRequest.assetName}</TableCell>
                                        <TableCell>{returnRequest.userName}</TableCell>
                                        <TableCell>{returnRequest.returnDate}</TableCell>
                                        <TableCell sx={{ color: returnRequest.returnStatusName === 'Sent' ? '#36A2EB' : returnRequest.returnStatusName === 'Approved' ? '#0BDA51' : returnRequest.returnStatusName === 'Returned' ? '#FF7518' : '#D2042D', fontWeight: 'bold' }}>{returnRequest.returnStatusName}</TableCell>
                                        <TableCell>
                                            <Link to={`/admin/return/${returnRequest.returnId}`}>
                                                <IconButton>
                                                    <InfoIcon />
                                                </IconButton>
                                            </Link>
                                        </TableCell>
                                    </TableRow>
                                ))
                            ) : (
                                <TableRow>
                                    <TableCell colSpan={10} align="center">No Requests found</TableCell>
                                </TableRow>
                            )}
                            </TableBody>
                        </Table>
                    </TableContainer>
                    <PaginationRounded count={pageCount} page={currentPage} onChange={(_, page) => setCurrentPage(page)} />
                </Box>
            </Box>
        </Box>
    );
}