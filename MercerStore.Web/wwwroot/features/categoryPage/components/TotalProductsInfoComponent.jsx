import React from 'react';
import {useCategoryProducts} from '../hooks/useCategoryProducts';

const TotalProductsInfoComponent = () => {
    const {totalProducts, pageNumber, pageSize} = useCategoryProducts();

    return (
        <div className="total-products">
            <h3>
                Показано {(pageNumber - 1) * pageSize + 1} - {Math.min(pageNumber * pageSize, totalProducts)} из {totalProducts}
            </h3>

        </div>
    );
};

export default TotalProductsInfoComponent;