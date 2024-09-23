import {useState} from 'react';

const usePagination = (itemsPerPage, items) =>{
    const[currentPage, setCurrentPage] = useState(1);

    const indexOfLastEmployee = currentPage * itemsPerPage;
    const indexOfFirstEmployee = indexOfLastEmployee - itemsPerPage;
    const currentItems = items.slice(indexOfFirstEmployee, indexOfLastEmployee);

    const pageCount = Math.ceil(items.length / itemsPerPage)

    const paginate = (event, value) => {
        setCurrentPage(value);
    };

    return {currentItems, paginate, pageCount};
}

export default usePagination;