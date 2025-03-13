import React, { useState, useRef, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useReviews } from '../hooks/useReviews';
import { updateReview } from '../redux/reviewSlice';


const UpdateReviewComponent = () => {
    const dispatch = useDispatch();
    const { review } = useReviews();


    const [showModal, setShowModal] = useState(false);
    const [reviewText, setReviewText] = useState(review?.reviewText || "");
    const [rating, setRating] = useState(review?.value);
    const modalRef = useRef(null);
    const modalInstance = useRef(null);


    useEffect(() => {
        if (review) {
            setReviewText(review?.reviewText || "");
            setRating(review?.value || 1);
        }
    }, [review]);

    const handleShow = () => {
        setReviewText(review.reviewText || "");
        setRating(review.value);
        setShowModal(true);
    };

    const handleClose = () => {
        if (modalInstance.current) {
            modalInstance.current.hide();
        }
        setShowModal(false);
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        const updatedReview = {
            ...review,
            productId: review.productId,
            reviewText: reviewText,
            value: rating,
        };

        dispatch(updateReview(updatedReview));
        handleClose();
    };

    useEffect(() => {
        if (showModal && modalRef.current) {
            modalInstance.current = new window.bootstrap.Modal(modalRef.current);
            modalInstance.current.show();

            modalRef.current.addEventListener('hidden.bs.modal', handleClose);

            return () => {

                modalRef.current.removeEventListener('hidden.bs.modal', handleClose);
                modalInstance.current = null;
            };
        }
    }, [showModal]);

    return (
        <>
            <button className="update-review-button" onClick={handleShow}>
                <i className="fa-solid fa-pen"></i>
            </button>

            <div className="modal fade" ref={modalRef} tabIndex="-1" aria-labelledby="modalLabel" aria-hidden="true">
                <div className="modal-dialog modal-lg modal-dialog-centered" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="modalLabel">Редактировать отзыв</h5>
                            <button type="button" className="btn-close" aria-label="Close" onClick={handleClose}></button>
                        </div>
                        <div className="modal-body">
                            <form onSubmit={handleSubmit}>
                                <div className="mb-3">
                                    <label htmlFor="reviewText" className="form-label">Опишите впечатления</label>
                                    <textarea
                                        id="reviewText"
                                        className="form-control"
                                        value={reviewText}
                                        onChange={(e) => setReviewText(e.target.value)}
                                        rows="3"
                                        placeholder="Место для отзыва"
                                    />
                                </div>
                                <div className="mb-3">
                                    <div className="update-review">
                                        <fieldset className="rating">
                                            <legend className="rating__caption">Оцените продукт</legend>
                                            <div className="rating__group">
                                                {[5, 4, 3, 2, 1].map((rate) => (
                                                    <React.Fragment key={rate}>
                                                        <input
                                                            type="radio"
                                                            id={`upd-rate${rate}`}
                                                            value={rate}
                                                            checked={rating === rate}
                                                            onChange={() => setRating(rate)}
                                                            className="rating__star-input"
                                                        />
                                                        <label htmlFor={`upd-rate${rate}`} className="rating__star">
                                                            <i className="fa-solid fa-star"></i>
                                                        </label>
                                                    </React.Fragment>
                                                ))}
                                            </div>
                                        </fieldset>
                                    </div>

                                </div>
                                <div className="modal-footer justify-content-start">
                                    <button type="submit" className="btn btn-outline-warning">
                                        Обновить отзыв
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default UpdateReviewComponent;