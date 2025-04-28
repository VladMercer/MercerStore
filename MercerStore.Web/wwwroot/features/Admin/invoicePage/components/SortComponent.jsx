import React from "react";
import {useDispatch} from "react-redux";
import {useInvoices} from "../hooks/useInvoices";
import {setSortOrder} from "../redux/invoicePageSlice";

const SortComponent = () => {
    const dispatch = useDispatch();
    const {sortOrder} = useInvoices();

    const handleSortChange = (event) => {
        dispatch(setSortOrder(event.target.value));
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Сортировка:</span>
            <select className="form-select" value={sortOrder} onChange={handleSortChange}>
                <option value="DateTimeAsc">Дата (сначала старые)</option>
                <option value="DateTimeDesc">Дата (сначала новые)</option>
                <option value="TotalAmountAsc">Сумма (по возрастанию)</option>
                <option value="TotalAmountDesc">Сумма (по убыванию)</option>
                <option value="StatusAsc">Статус (А-Я)</option>
                <option value="StatusDesc">Статус (Я-А)</option>
            </select>
        </div>
    );
};

export default SortComponent;