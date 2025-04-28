import React, {useContext, useEffect} from 'react';
import {ProductContext} from './ProductContext';
import PaginationComponent from '../../UI/PaginationComponent';

const PaginationApp = () => {
    const {pageNumber, setPageNumber, totalPages, fetchProducts} = useContext(ProductContext);

    useEffect(() => {
        fetchProducts();
    }, [pageNumber]);

    const handlePageChange = (newPageNumber) => {
        if (newPageNumber > 0 && newPageNumber <= totalPages) {
            setPageNumber(newPageNumber);
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