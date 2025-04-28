import React from "react";
import {useDispatch} from "react-redux";
import {useReviews} from "../hooks/useReviews";
import {setSortOrder} from "../redux/reviewPageSlice";

const SortComponent = () => {
    const dispatch = useDispatch();
    const {sortOrder} = useReviews();

    const handleSortChange = (event) => {
        dispatch(setSortOrder(event.target.value));
    };

    return (
        <div className="input-group mb-3">
            <span className="input-group-text">Сортировка:</span>
            <select className="form-select" value={sortOrder} onChange={handleSortChange}>
                <option value="CreateDateDesc">Сначала новые</option>
                <option value="CreateDateAsc">Сначала старые</option>
                <option value="NameAsc">Имя (А-Я)</option>
                <option value="NameDesc">Имя (Я-А)</option>
                <option value="EditDateAsc">Редактированы недавно</option>
                <option value="EditDateDesc">Редактированы давно</option>
                <option value="ValueAsc">Оценка (1-5)</option>
                <option value="ValueDesc">Оценка (5-1)</option>
            </select>
        </div>
    );
};

export default SortComponent;