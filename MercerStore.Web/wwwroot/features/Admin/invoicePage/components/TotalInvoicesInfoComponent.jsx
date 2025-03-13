import React from 'react';
import { useInvoices } from '../hooks/useInvoices';

const TotalInvoicesInfoComponent = () => {
    const { totalInvoices, pageNumber, pageSize } = useInvoices();

    return (
        <div className="total-products">
            <h3>
                Показано {(pageNumber - 1) * pageSize + 1} - {Math.min(pageNumber * pageSize, totalInvoices)} из {totalInvoices}
            </h3>
           
        </div>
    );
};

export default TotalInvoicesInfoComponent;