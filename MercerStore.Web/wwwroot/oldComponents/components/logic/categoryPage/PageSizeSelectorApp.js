import React, {useContext, useEffect} from 'react';
import {ProductContext} from './ProductContext';
import PageSizeSelectorComponent from '../../UI/PageSizeSelectorComponent';

const PageSizeSelectorApp = () => {
    const {pageSize, setPageSize, fetchProducts} = useContext(ProductContext);

    const handlePageSizeChange = (event) => {
        const newSize = parseInt(event.target.value);
        setPageSize(newSize);
    };
    useEffect(() => {
        fetchProducts();
    }, [pageSize]);
    return (
        <PageSizeSelectorComponent pageSize={pageSize} onChange={handlePageSizeChange}/>
    );
};

export default PageSizeSelectorApp;