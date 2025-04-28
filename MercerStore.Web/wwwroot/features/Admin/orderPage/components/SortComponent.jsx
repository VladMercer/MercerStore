import React from 'react';
import {useDispatch} from 'react-redux';
import {useOrders} from '../hooks/useOrders';
import {setSortOrder} from '../redux/orderPageSlice';

const SortComponent = () => {
    const dispatch = useDispatch();
    const {sortOrder} = useOrders();

    const handleSortChange = (event) => {
        const newSortOrder = event.target.value;
        dispatch(setSortOrder(newSortOrder));
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Сортировать:</span>
            <select className="form-select" value={sortOrder} onChange={handleSortChange}>
                <option value="DateTimeAsc">сначала новые</option>
                <option value="DateTimeDesc">Сначала старые</option>
                <option value="TotalPriceAsc">Сначала недорогие</option>
                <option value="TotalPriceDesc">Сначала дорогие</option>
                <option value="StatusAsc">Сначала на рассмотрении</option>
                <option value="StatusDesc">Сначала Отмененные</option>
            </select>
        </div>
    );
};

export default SortComponent;