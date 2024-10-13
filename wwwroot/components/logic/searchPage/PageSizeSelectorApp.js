import React, { useContext } from 'react';
import PageSizeSelectorComponent from '../../UI/PageSizeSelectorComponent';
import { SearchContext } from './SearchContext';

const PageSizeSelectorApp = () => {
    const { pageSize, setPageSize, fetchProducts } = useContext(SearchContext);

    const handlePageSizeChange = (event) => {
        const newSize = parseInt(event.target.value, 10);
        setPageSize(newSize);
        fetchProducts();  
    };

    return (
        <PageSizeSelectorComponent pageSize={pageSize} onChange={handlePageSizeChange} />
    );
};

export default PageSizeSelectorApp;