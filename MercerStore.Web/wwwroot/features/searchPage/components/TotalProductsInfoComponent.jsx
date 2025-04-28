import React from 'react';
import {useSearchPage} from '../hooks/useSearchPage';

const TotalProductsInfoComponent = () => {
    const {totalItems, pageNumber, pageSize} = useSearchPage();

    return (
        <div className="total-products">
            Показано {(pageNumber - 1) * pageSize + 1} - {Math.min(pageNumber * pageSize, totalItems)} из {totalItems}
        </div>
    );
};

export default TotalProductsInfoComponent;