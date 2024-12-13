import React from 'react';
import { useCategoryProducts } from '../hooks/useCategoryProducts';

const TotalProductsInfoComponent = () => {
    const { totalProducts, pageNumber, pageSize } = useCategoryProducts();

    return (
        <div className="total-products">
            Показано {(pageNumber - 1) * pageSize + 1} - {Math.min(pageNumber * pageSize, totalProducts)} из {totalProducts}
        </div>
    );
};

export default TotalProductsInfoComponent;