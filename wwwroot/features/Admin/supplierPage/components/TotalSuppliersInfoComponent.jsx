import React from 'react';
import { useSuppliers } from '../hooks/useSuppliers';

const TotalSuppliersInfoComponent = () => {
    const { totalSuppliers, pageNumber, pageSize } = useSuppliers();

    return (
        <div className="total-products">
            <h3>
                Показано {(pageNumber - 1) * pageSize + 1} - {Math.min(pageNumber * pageSize, totalSuppliers)} из {totalSuppliers}
            </h3>
           
        </div>
    );
};

export default TotalSuppliersInfoComponent;