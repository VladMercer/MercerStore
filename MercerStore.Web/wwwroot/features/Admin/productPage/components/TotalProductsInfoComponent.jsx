import React from 'react';
import {useProducts} from '../hooks/useProducts';

const TotalProductsInfoComponent = () => {
    const {totalProducts, pageNumber, pageSize} = useProducts();

    return (
        <div className="total-products">
            <h3>
                Показано {(pageNumber - 1) * pageSize + 1} - {Math.min(pageNumber * pageSize, totalProducts)} из {totalProducts}
            </h3>

        </div>
    );
};

export default TotalProductsInfoComponent;