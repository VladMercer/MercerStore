import React, { useContext } from 'react';
import PaginationComponent from '../../UI/PaginationComponent';
import { SearchContext } from './SearchContext';

const PaginationApp = () => {
    const { pageNumber, setPageNumber, totalPages, fetchProducts } = useContext(SearchContext);

    const handlePageChange = (newPageNumber) => {
        if (newPageNumber > 0 && newPageNumber <= totalPages) {
            setPageNumber(newPageNumber);
            fetchProducts();  
        }
    };

    return (
        <PaginationComponent
            pageNumber={pageNumber}
            totalPages={totalPages}
            onPageChange={handlePageChange}
        />
    );
};

export default PaginationApp;