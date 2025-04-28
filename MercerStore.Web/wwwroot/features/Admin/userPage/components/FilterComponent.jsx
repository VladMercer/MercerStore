import React from "react";
import {useDispatch} from "react-redux";
import {useUsers} from "../hooks/useUsers";
import {setFilter} from "../redux/userPageSlice";

const FilterComponent = () => {
    const dispatch = useDispatch();
    const {statusFilter} = useUsers();

    const handleFilterChange = (event) => {
        dispatch(setFilter(event.target.value));
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Фильтр:</span>
            <select className="form-select" value={statusFilter} onChange={handleFilterChange}>
                <option value="">Все пользователи</option>
                <option value="Online">Онлайн</option>
                <option value="Offline">Оффлайн</option>
                <option value="User">Обычные пользователи</option>
                <option value="Manager">Менеджеры</option>
                <option value="Admin">Администраторы</option>
                <option value="Banned">Заблокированные</option>
                <option value="HasReview">С отзывами</option>
                <option value="NoReview">Без отзывов</option>
                <option value="HasOrder">С заказами</option>
                <option value="NoOrder">Без заказов</option>
                <option value="CreateDate">Дата создания</option>
                <option value="LastActivityDate">Дата активности</option>
            </select>
        </div>
    );
};

export default FilterComponent;