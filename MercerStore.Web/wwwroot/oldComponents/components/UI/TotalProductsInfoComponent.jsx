import React from 'react';

const TotalProductsInfoComponent = ({totalProducts, pageNumber, pageSize}) => {
    return (
        <div className="total-products">
            Показано {(pageNumber - 1) * pageSize + 1} - {Math.min(pageNumber * pageSize, totalProducts)} из {totalProducts}
        </div>
    );
};

export default TotalProductsInfoComponent;