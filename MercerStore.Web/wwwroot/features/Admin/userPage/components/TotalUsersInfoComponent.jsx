import React from 'react';
import {useUsers} from '../hooks/useUsers';

const TotalUsersInfoComponent = () => {
    const {totalUsers, pageNumber, pageSize} = useUsers();

    return (
        <div className="total-products">
            <h3>
                Показано {(pageNumber - 1) * pageSize + 1} - {Math.min(pageNumber * pageSize, totalUsers)} из {totalUsers}
            </h3>

        </div>
    );
};

export default TotalUsersInfoComponent;