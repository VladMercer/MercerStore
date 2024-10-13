import React, { useContext, useEffect, useState, useRef } from 'react';
import { ProductContext } from './ProductContext';
import FilterComponent from '../../UI/FilterComponent';
const FilterApp = () => {
    const {
        fetchProducts,
        minPrice,
        maxPrice,
        setSelectedMinPrice,
        setSelectedMaxPrice,
    } = useContext(ProductContext);

    const [localMinPrice, setLocalMinPrice] = useState(minPrice);
    const [localMaxPrice, setLocalMaxPrice] = useState(maxPrice);
    const debounceTimeout = useRef(null);

    const debounceFetchProducts = (min, max) => {
        if (debounceTimeout.current) {
            clearTimeout(debounceTimeout.current);
        }

        debounceTimeout.current = setTimeout(() => {
            setSelectedMinPrice(min);
            setSelectedMaxPrice(max);

            fetchProducts(min, max); 
        }, 500);
    };

    const handleMinPriceChange = (newMinPrice) => {
        setLocalMinPrice(newMinPrice); 
        debounceFetchProducts(newMinPrice, localMaxPrice); 
    };

    const handleMaxPriceChange = (newMaxPrice) => {
        setLocalMaxPrice(newMaxPrice); 
        debounceFetchProducts(localMinPrice, newMaxPrice); 
    };

    useEffect(() => {
        setLocalMinPrice(minPrice); 
        setLocalMaxPrice(maxPrice); 
    }, [minPrice, maxPrice]); 

    return (
        <FilterComponent
            minPrice={minPrice}
            maxPrice={maxPrice}
            selectedMinPrice={localMinPrice} 
            selectedMaxPrice={localMaxPrice} 
            onMinPriceChange={handleMinPriceChange}
            onMaxPriceChange={handleMaxPriceChange}
        />
    );
};

export default FilterApp;