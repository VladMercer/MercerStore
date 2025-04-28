import React, {useContext, useEffect} from 'react';
import {ProductContext} from './ProductContext';
import SortComponent from '../../UI/SortComponent';

const SortApp = () => {
    const {sortOrder, setSortOrder, fetchProducts} = useContext(ProductContext);

    const handleSortChange = (event) => {
        const newSortOrder = event.target.value;
        setSortOrder(newSortOrder);
    };
    useEffect(() => {
        fetchProducts();
    }, [sortOrder]);
    return (
        <SortComponent currentSort={sortOrder} onChange={handleSortChange}/>
    );
};

export default SortApp;