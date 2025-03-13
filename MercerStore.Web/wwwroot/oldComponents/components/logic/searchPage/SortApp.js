import React, { useContext } from 'react';
import SortComponent from '../../UI/SortComponent';
import { SearchContext } from './SearchContext';

const SortApp = () => {
    const { sortOrder, setSortOrder, fetchProducts } = useContext(SearchContext);

    const handleSortChange = (event) => {
        const newSortOrder = event.target.value;
        setSortOrder(newSortOrder);
        fetchProducts();  
    };

    return (
        <SortComponent currentSort={sortOrder} onChange={handleSortChange} />
    );
};

export default SortApp;