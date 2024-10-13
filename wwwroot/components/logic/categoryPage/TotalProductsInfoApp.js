import React, { useContext, useEffect } from 'react';
import { ProductContext } from './ProductContext';
import TotalProductsInfoComponent from '../../UI/TotalProductsInfoComponent';
const TotalProductsInfoApp = () => {
    const { totalProducts, pageSize, pageNumber, fetchProducts } = useContext(ProductContext);

    useEffect(() => {
        fetchProducts(); 
    }, []);

    return (
        <TotalProductsInfoComponent
            totalProducts={totalProducts}
            pageNumber={pageNumber}
            pageSize={pageSize}
        />
    );
};

export default TotalProductsInfoApp;