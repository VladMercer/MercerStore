import React from "react";
import {useDispatch} from "react-redux";
import {useOrders} from "../hooks/useOrders";
import {setStatusFilter} from "../redux/orderPageSlice";

const StatusFilterComponent = () => {
    const dispatch = useDispatch();
    const {statusFilter} = useOrders();

    const handleFilterChange = (event) => {
        const newFilter = event.target.value;
        dispatch(setStatusFilter(newFilter));
    };
    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Статус:</span>
            <select className="form-select" value={statusFilter} onChange={handleFilterChange}>
                <option value="All">Все</option>
                <option value="Pending">На рассмотрении</option>
                <option value="InProgress">В процессе</option>
                <option value="Completed">Исполненные</option>
                <option value="Cancelled">Отмененные</option>
                <option value="Failed">Неудачные</option>
            </select>
        </div>
    );
};

export default StatusFilterComponent;