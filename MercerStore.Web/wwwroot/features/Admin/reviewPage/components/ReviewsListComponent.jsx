import React, {useMemo} from "react";
import {useDispatch} from "react-redux";
import {useReviews} from "../hooks/useReviews";
import useFetchReviews from "../hooks/useFetchReviews";
import {fetchReviews, removeReview} from "../redux/reviewPageSlice";

const ReviewsListComponent = () => {
    useFetchReviews();
    const dispatch = useDispatch();
    const {reviews, pageNumber, pageSize, sortOrder, timePeriodFilter, filter, query} = useReviews();

    const handleDelete = async (reviewId) => {
        if (window.confirm("Вы уверены, что хотите удалить этот отзыв?")) {
            await dispatch(removeReview(reviewId)).unwrap();
            dispatch(fetchReviews({pageNumber, pageSize, sortOrder, timePeriodFilter, filter, query}));
        }
    };

    const renderReviews = useMemo(() => {
        if (!reviews || reviews.length === 0) {
            return (
                <tr>
                    <td colSpan="6" className="text-center">
                        Нет данных
                    </td>
                </tr>
            );
        }

        return reviews.map((review) => (
            <tr
                key={review.id + review.userId}
                style={{cursor: "pointer", verticalAlign: "middle"}}
                onClick={() => (window.location.href = `/admin/user/update/${review.userId}`)}>

                <td className="text-center">
                    {review.productImgUrl ? (
                        <img src={review.productImgUrl} alt="Product" width="50" height="50"/>
                    ) : (
                        <span className="text-muted">—</span>
                    )}
                </td>

                <td className="text-start">
                    <div>
                        <strong>Пользователь:</strong> {review.userName}
                    </div>
                    <div>
                        <strong>Email:</strong> {review.email}
                    </div>
                </td>

                <td className="text-start">
                    <div>
                        <strong>Создан:</strong> {new Date(review.date).toLocaleDateString()}
                    </div>
                    <div>
                        <strong>Изменён:</strong> {new Date(review.editDate).toLocaleDateString()}
                    </div>
                </td>

                <td className="text-start">
                    <div>
                        <strong>Оценка:</strong> {review.value}
                    </div>
                    <div>
                        <strong>Отзыв:</strong> {review.reviewText ? review.reviewText : "—"}
                    </div>
                </td>

                <td className="text-center">
                    <button
                        className="remove-from-cart-button"
                        onClick={(e) => {
                            e.stopPropagation();
                            handleDelete(review.id);
                        }}
                    >
                        <i className="fa-solid fa-trash-can"></i>
                    </button>
                </td>
            </tr>
        ));
    }, [reviews]);

    return (
        <table className="table table-striped table-hover table-responsive-md table-sm">
            <thead className="thead-dark">
            <tr>
                <th className="text-center">Фото</th>
                <th className="text-center">Пользователь</th>
                <th className="text-center">Даты</th>
                <th className="text-center">Оценка и текст</th>
                <th className="text-center"></th>
            </tr>
            </thead>
            <tbody>{renderReviews}</tbody>
        </table>
    );
};

export default ReviewsListComponent;