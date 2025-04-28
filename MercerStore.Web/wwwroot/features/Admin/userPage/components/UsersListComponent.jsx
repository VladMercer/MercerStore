import React, {useMemo} from 'react';
import {useUsers} from '../hooks/useUsers';
import useFetchUsers from '../hooks/useFetchUsers';

const UsersListComponent = () => {
    useFetchUsers();
    const {users} = useUsers();

    const renderUsers = useMemo(() => {
        if (!users || users.length === 0) {
            return (
                <tr>
                    <td colSpan="6" className="text-center">
                        Нет данных
                    </td>
                </tr>
            );
        }

        return users.map((user) => (
            <tr
                key={user.id}
                onClick={() => (window.location.href = `/admin/user/update/${user.id}`)}
                style={{cursor: 'pointer', verticalAlign: 'middle'}}
            >
                <td className="text-center">
                    {user.imageUrl ? (
                        <img src={user.imageUrl} alt="User" className="rounded-circle" width="50" height="50"/>
                    ) : (
                        <span className="text-muted">—</span>
                    )}
                </td>

                <td className="text-start">
                    <div>
                        <strong>Email:</strong> {user.email || '—'}
                    </div>
                    <div>
                        <strong>Телефон:</strong> {user.phoneNumber || '—'}
                    </div>
                    <div>
                        <strong>Адрес:</strong> {user.address || '—'}
                    </div>
                </td>

                <td className="text-start">
                    <div>
                        <strong>Создан:</strong> {new Date(user.createDate).toLocaleDateString()}
                    </div>
                    {user.lastActivityDate && (
                        <div>
                            <strong>Активность:</strong> {new Date(user.lastActivityDate).toLocaleDateString()}
                        </div>
                    )}
                </td>

                <td className="text-start">
                    <div>
                        <strong>Заказы:</strong> {user.countOrders ?? 0}
                    </div>
                    <div>
                        <strong>Отзывы:</strong> {user.countReviews ?? 0}
                    </div>
                </td>

                <td className="text-start">
                    <div>
                        <strong>Роли:</strong>{' '}
                        {user.roles && user.roles.length > 0 ? user.roles.join(', ') : '—'}
                    </div>
                    <div>
                        <strong>ID:</strong> {user.id || '—'}
                    </div>
                </td>
            </tr>
        ));
    }, [users]);

    return (
        <table className="table table-striped table-hover table-responsive-md table-sm">
            <thead className="thead-dark">
            <tr>
                <th className="text-center">Фото</th>
                <th className="text-center">Контакты</th>
                <th className="text-center">Даты</th>
                <th className="text-center">Статистика</th>
                <th className="text-center">Роли</th>
            </tr>
            </thead>
            <tbody>{renderUsers}</tbody>
        </table>
    );
};

export default UsersListComponent;