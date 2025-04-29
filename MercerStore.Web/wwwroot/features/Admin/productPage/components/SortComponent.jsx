import React from 'react';
import {useDispatch} from 'react-redux';
import {useProducts} from '../hooks/useProducts';
import {setSortOrder} from '../redux/adminProductPageSlice';

const SortComponent = () => {
    const dispatch = useDispatch();
    const {sortOrder} = useProducts();

    const handleSortChange = (event) => {
        const newSortOrder = event.target.value;
        dispatch(setSortOrder(newSortOrder));
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Сортировать:</span>
            <select className="form-select" value={sortOrder} onChange={handleSortChange}>
                <option value="NameAsc">Имя (я-а)</option>
                <option value="NameDesc">Имя (а-я)</option>
                <option value="PriceAsc">Сначала недорогие</option>
                <option value="PriceDesc">Сначала дорогие</option>
                <option value="StatusAsc">Сначала доступные</option>
                <option value="StatusDesc">Сначала недоступные</option>
                <option value="InStockAsc">Сначала заканчивающиеся</option>
                <option value="InStockDesc">Сначала больше всего</option>
            </select>
        </div>
    );
};

export default SortComponent;