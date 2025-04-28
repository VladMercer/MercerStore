import React from "react";
import {useDispatch} from "react-redux";
import {useInvoices} from "../hooks/useInvoices";
import {setFilter} from "../redux/invoicePageSlice";

const FilterComponent = () => {
    const dispatch = useDispatch();
    const {filter} = useInvoices();

    const handleFilterChange = (event) => {
        dispatch(setFilter(event.target.value));
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Фильтр:</span>
            <select className="form-select" value={filter} onChange={handleFilterChange}>
                <option value="All">Все поставки</option>
                <option value="Pending">В ожидании</option>
                <option value="Received">Получено</option>
                <option value="PartiallyReceived">Частично получено</option>
                <option value="Rejected">Отклонено</option>
                <option value="Closed">Закрыто</option>
                <option value="EditDate">По дате редактирования</option>
                <option value="DateReceived">По дате получения</option>
                <option value="PaymentDate">По дате оплаты</option>
            </select>
        </div>
    );
};

export default FilterComponent;