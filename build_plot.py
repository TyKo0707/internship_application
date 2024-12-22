import numpy as np
import pandas as pd
import seaborn as sns
import matplotlib.pyplot as plt


def plot_results(args):
    """ Plot the results of the evaluation using simple lineplot. """
    method = args['method']
    df = pd.read_csv(args['input_path'])
    df['S_interval'] = df['S_interval'].apply(lambda x: int(x.split('-')[1]))
    grouped = df.groupby('S_interval')['TurnsCount'].agg(['max', 'mean']).reset_index()
    grouped['max'][grouped['max'] > 35e6] = 35e6
    grouped['mean'][grouped['mean'] > 35e6] = 35e6
    y = grouped['S_interval']

    max_ratio = 0
    for ind, row in grouped.iterrows():
        ratio = row['max'] / row['S_interval']
        if ratio > max_ratio:
            max_ratio = ratio
    max_ratio = int(max_ratio) + 1

    sns.set(style="whitegrid")
    plt.figure(figsize=(12, 6))
    plt.plot(y, y * 35, color='r', linestyle='--', label='35S')
    if max_ratio <= 35:
        plt.plot(y, y * max_ratio, color='y', linestyle='--', label=f'{max_ratio}S')
    plt.plot(y, grouped['max'], label='Maximum # of moves', color='blue')
    plt.plot(y, grouped['mean'], label='Average # of moves', color='green')
    plt.xlabel('S')
    plt.ylabel('# of Turns')
    plt.title(f'Maximum and Average Turns for each S ({method} method)')
    plt.legend()
    plt.savefig(args['output_path'], dpi=300)
    plt.show()


if __name__ == '__main__':
    args = {'method': 'lcm_zigzag'}
    args['input_path'] = f'./data/results/{args["method"]}_results.csv'
    args['output_path'] = f'./img/{args["method"]}_results.png'
    plot_results(args)
