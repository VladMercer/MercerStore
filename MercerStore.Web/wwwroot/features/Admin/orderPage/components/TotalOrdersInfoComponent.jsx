import React from 'react';
import {useOrders} from '../hooks/useOrders';

const TotalOrdersInfoComponent = () => {
    const {totalOrders, pageNumber, pageSize} = useOrders();

    return (
        <div className="total-products">
            <h3>
                Показано {(pageNumber - 1) * pageSize + 1} - {Math.min(pageNumber * pageSize, totalOrders)} из {totalOrders}
            </h3>

        </div>
    );
};

export default TotalOrdersInfoComponent;